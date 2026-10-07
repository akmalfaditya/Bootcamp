import { useState, useEffect, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import {
  ConnectionStatus,
  ChatMessage,
  UseSignalROptions,
  UseSignalRReturn,
  normalizeChannelId,
  createChatMessage,
  DEFAULT_CHANNEL_ID,
} from '../types/chat';

export type { ConnectionStatus, ChatMessage, UseSignalROptions, UseSignalRReturn };

export interface MessageDto {
  user: string;
  message: string;
}

export const useSignalR = (
  urlOrOptions?: string | UseSignalROptions
): UseSignalRReturn => {
  const options: UseSignalROptions =
    typeof urlOrOptions === 'string'
      ? { url: urlOrOptions }
      : urlOrOptions || {};

  const targetUrl = options.url || '/hubs/chat';
  const defaultChannel = options.defaultChannel
    ? normalizeChannelId(options.defaultChannel)
    : (options.activeChannel ? normalizeChannelId(options.activeChannel) : DEFAULT_CHANNEL_ID);

  const [connectionStatus, setConnectionStatus] = useState<ConnectionStatus>('Disconnected');
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [subscribedGroupsState, setSubscribedGroupsState] = useState<string[]>([defaultChannel]);

  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const subscribedGroupsRef = useRef<Set<string>>(new Set([defaultChannel]));
  const isRestApiCheck = options.isRestApiPredicate;

  // Sync state array with Set for consumers
  const syncSubscribedGroups = useCallback(() => {
    setSubscribedGroupsState(Array.from(subscribedGroupsRef.current));
  }, []);

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  const clearMessages = useCallback(() => {
    setMessages([]);
  }, []);

  useEffect(() => {
    let isCancelled = false;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(targetUrl)
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connectionRef.current = connection;

    // 1. Incoming global broadcast message (ReceiveMessage)
    const handleReceiveMessage = (user: string, message: string) => {
      const isRest = isRestApiCheck ? isRestApiCheck(user, message) : undefined;
      const newMsg = createChatMessage({
        user,
        message,
        channel: defaultChannel,
        isRestApi: isRest,
      });
      setMessages((prev) => [...prev, newMsg]);
    };

    // 2. Incoming room-isolated group message (ReceiveGroupMessage)
    const handleReceiveGroupMessage = (groupName: string, user: string, message: string) => {
      const isRest = isRestApiCheck ? isRestApiCheck(user, message) : undefined;
      const newMsg = createChatMessage({
        user,
        message,
        channel: groupName,
        isRestApi: isRest,
      });
      setMessages((prev) => [...prev, newMsg]);
    };

    connection.on('ReceiveMessage', handleReceiveMessage);
    connection.on('ReceiveGroupMessage', handleReceiveGroupMessage);

    // Lifecycle: Reconnecting
    connection.onreconnecting((err) => {
      setConnectionStatus('Reconnecting');
      setError(err ? err.message : 'Reconnecting to chat server...');
    });

    // Lifecycle: Reconnected -> Automatically resubscribe to active groups
    connection.onreconnected(async () => {
      setConnectionStatus('Connected');
      setError(null);

      const groupsToRejoin = Array.from(subscribedGroupsRef.current);
      for (const group of groupsToRejoin) {
        try {
          await connection.invoke('JoinGroup', group);
        } catch (rejoinErr) {
          console.error(`Failed to rejoin group "${group}" after reconnect:`, rejoinErr);
        }
      }
    });

    // Lifecycle: Closed
    connection.onclose((err) => {
      setConnectionStatus('Disconnected');
      if (err) {
        setError(err.message || 'SignalR connection lost.');
      }
    });

    // Start connection with StrictMode safety
    const startConnection = async () => {
      try {
        await connection.start();
        if (isCancelled) {
          await connection.stop();
          return;
        }

        setConnectionStatus('Connected');
        setError(null);

        // Join initial subscribed groups
        const initialGroups = Array.from(subscribedGroupsRef.current);
        for (const group of initialGroups) {
          try {
            await connection.invoke('JoinGroup', group);
          } catch (joinErr) {
            console.error(`Failed to join initial group "${group}":`, joinErr);
          }
        }
      } catch (err) {
        if (!isCancelled) {
          setConnectionStatus('Disconnected');
          const errorMessage = err instanceof Error ? err.message : 'Failed to connect to SignalR hub.';
          setError(errorMessage);
        }
      }
    };

    startConnection();

    // Cleanup on unmount
    return () => {
      isCancelled = true;
      connection.off('ReceiveMessage', handleReceiveMessage);
      connection.off('ReceiveGroupMessage', handleReceiveGroupMessage);

      if (connectionRef.current === connection) {
        connectionRef.current = null;
      }

      if (connection.state !== signalR.HubConnectionState.Disconnected) {
        connection.stop().catch((stopErr) => {
          console.warn('Error during SignalR connection stop cleanup:', stopErr);
        });
      }
    };
  }, [targetUrl, defaultChannel, isRestApiCheck]);

  // Invocation: SendMessage (Global broadcast)
  const sendMessage = useCallback(async (user: string, message: string) => {
    if (!user.trim() || !message.trim()) {
      throw new Error('User and message cannot be empty.');
    }
    const conn = connectionRef.current;
    if (!conn || conn.state !== signalR.HubConnectionState.Connected) {
      const msg = `Cannot send message: Chat server is not connected (current state: ${conn?.state ?? 'Disconnected'}).`;
      setError(msg);
      throw new Error(msg);
    }

    try {
      await conn.invoke('SendMessage', user.trim(), message.trim());
    } catch (err) {
      const msg = err instanceof Error ? err.message : 'Error sending broadcast message.';
      setError(msg);
      throw err;
    }
  }, []);

  // Invocation: SendMessageToGroup (Channel-isolated broadcast)
  const sendMessageToGroup = useCallback(async (groupName: string, user: string, message: string) => {
    const normalizedGroup = normalizeChannelId(groupName);
    if (!normalizedGroup) {
      throw new Error('Group name cannot be empty.');
    }
    if (!user.trim() || !message.trim()) {
      throw new Error('User and message cannot be empty.');
    }
    const conn = connectionRef.current;
    if (!conn || conn.state !== signalR.HubConnectionState.Connected) {
      const msg = `Cannot send message to group: Chat server is not connected (current state: ${conn?.state ?? 'Disconnected'}).`;
      setError(msg);
      throw new Error(msg);
    }

    try {
      await conn.invoke('SendMessageToGroup', normalizedGroup, user.trim(), message.trim());
    } catch (err) {
      const msg = err instanceof Error ? err.message : `Error sending message to group ${normalizedGroup}.`;
      setError(msg);
      throw err;
    }
  }, []);

  // Invocation: JoinGroup
  const joinGroup = useCallback(async (groupName: string) => {
    const normalizedGroup = normalizeChannelId(groupName);
    if (!normalizedGroup) return;

    subscribedGroupsRef.current.add(normalizedGroup);
    syncSubscribedGroups();

    const conn = connectionRef.current;
    if (conn && conn.state === signalR.HubConnectionState.Connected) {
      try {
        await conn.invoke('JoinGroup', normalizedGroup);
      } catch (err) {
        const msg = err instanceof Error ? err.message : `Failed to join group ${normalizedGroup}.`;
        setError(msg);
        throw err;
      }
    }
  }, [syncSubscribedGroups]);

  // Invocation: LeaveGroup
  const leaveGroup = useCallback(async (groupName: string) => {
    const normalizedGroup = normalizeChannelId(groupName);
    if (!normalizedGroup) return;

    subscribedGroupsRef.current.delete(normalizedGroup);
    syncSubscribedGroups();

    const conn = connectionRef.current;
    if (conn && conn.state === signalR.HubConnectionState.Connected) {
      try {
        await conn.invoke('LeaveGroup', normalizedGroup);
      } catch (err) {
        const msg = err instanceof Error ? err.message : `Failed to leave group ${normalizedGroup}.`;
        setError(msg);
        throw err;
      }
    }
  }, [syncSubscribedGroups]);

  // Fallback Manual Reconnect
  const reconnect = useCallback(async () => {
    const conn = connectionRef.current;
    if (!conn) {
      throw new Error('No connection instance available to reconnect.');
    }
    if (conn.state === signalR.HubConnectionState.Connected) {
      return;
    }

    setError(null);
    try {
      await conn.start();
      setConnectionStatus('Connected');

      // Re-join subscribed groups
      const groups = Array.from(subscribedGroupsRef.current);
      for (const group of groups) {
        await conn.invoke('JoinGroup', group);
      }
    } catch (err) {
      const msg = err instanceof Error ? err.message : 'Manual reconnection failed.';
      setError(msg);
      throw err;
    }
  }, []);

  return {
    connectionStatus,
    connectionState: connectionStatus,
    messages,
    error,
    subscribedGroups: subscribedGroupsState,
    isConnected: connectionStatus === 'Connected',
    sendMessage,
    sendMessageToGroup,
    joinGroup,
    leaveGroup,
    clearMessages,
    clearError,
    reconnect,
  };
};
