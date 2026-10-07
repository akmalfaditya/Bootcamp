import { useState, useCallback, useMemo, useEffect, useRef } from 'react';
import { useSignalR } from '@/hooks/useSignalR';
import { STORAGE_KEYS, DEFAULT_CHANNELS, DEFAULT_CHANNEL_ID, normalizeChannelId, type Channel } from '@/types/chat';
import { ChatLayout } from '@/components/chat/ChatLayout';
import { OnboardingModal } from '@/components/OnboardingModal';
import { ProfileModal } from '@/components/ProfileModal';

function readStoredUsername(): string {
  try {
    const stored = localStorage.getItem(STORAGE_KEYS.USERNAME);
    return stored && stored.trim().length >= 2 ? stored.trim() : '';
  } catch {
    return '';
  }
}

function readStoredChannel(): string {
  try {
    const stored = localStorage.getItem(STORAGE_KEYS.ACTIVE_CHANNEL);
    if (stored && DEFAULT_CHANNELS.some((c) => c.id === normalizeChannelId(stored))) {
      return normalizeChannelId(stored);
    }
  } catch {
    // Storage unavailable; fall back to the default channel.
  }
  return DEFAULT_CHANNEL_ID;
}

function App() {
  const [currentUser, setCurrentUser] = useState<string>(readStoredUsername);
  const [activeChannelId, setActiveChannelId] = useState<string>(readStoredChannel);
  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const [unreadCounts, setUnreadCounts] = useState<Record<string, number>>({});

  const { connectionState, messages, sendMessage, sendMessageToGroup, joinGroup, leaveGroup } = useSignalR();

  const isOnboardingRequired = currentUser.trim().length < 2;

  const activeChannel = useMemo<Channel>(
    () => DEFAULT_CHANNELS.find((c) => c.id === activeChannelId) ?? DEFAULT_CHANNELS[0],
    [activeChannelId]
  );

  // Ensure the active channel group is joined whenever the connection (re)establishes.
  useEffect(() => {
    if (connectionState === 'Connected' && activeChannelId !== DEFAULT_CHANNEL_ID) {
      joinGroup(activeChannelId).catch((err: unknown) => {
        console.warn('Failed to join active channel group:', err);
      });
    }
  }, [connectionState, activeChannelId, joinGroup]);

  // Count unread messages for channels the user is not currently viewing.
  const seenCountRef = useRef(messages.length);
  useEffect(() => {
    if (messages.length <= seenCountRef.current) {
      seenCountRef.current = messages.length;
      return;
    }
    const fresh = messages.slice(seenCountRef.current);
    seenCountRef.current = messages.length;

    const increments: Record<string, number> = {};
    for (const msg of fresh) {
      const channel = normalizeChannelId(msg.channel);
      if (channel && channel !== activeChannelId) {
        increments[channel] = (increments[channel] ?? 0) + 1;
      }
    }
    if (Object.keys(increments).length > 0) {
      setUnreadCounts((prev) => {
        const next = { ...prev };
        for (const [channel, count] of Object.entries(increments)) {
          next[channel] = (next[channel] ?? 0) + count;
        }
        return next;
      });
    }
  }, [messages, activeChannelId]);

  const channelsWithUnread = useMemo<readonly Channel[]>(
    () => DEFAULT_CHANNELS.map((ch) => ({ ...ch, unreadCount: unreadCounts[ch.id] ?? 0 })),
    [unreadCounts]
  );

  const activeMessages = useMemo(
    () => messages.filter((msg) => normalizeChannelId(msg.channel) === activeChannelId),
    [messages, activeChannelId]
  );

  const saveUsername = useCallback((username: string) => {
    const trimmed = username.trim();
    try {
      localStorage.setItem(STORAGE_KEYS.USERNAME, trimmed);
    } catch (err) {
      console.error('Failed to persist display name:', err);
    }
    setCurrentUser(trimmed);
  }, []);

  const handleProfileSave = useCallback(
    (username: string) => {
      saveUsername(username);
      setIsProfileOpen(false);
    },
    [saveUsername]
  );

  const handleSelectChannel = useCallback(
    async (channelId: string) => {
      const next = normalizeChannelId(channelId);
      if (next === activeChannelId) return;

      try {
        await leaveGroup(activeChannelId);
        await joinGroup(next);
      } catch (err) {
        console.error('Failed to switch channel groups:', err);
      }

      setActiveChannelId(next);
      try {
        localStorage.setItem(STORAGE_KEYS.ACTIVE_CHANNEL, next);
      } catch {
        // Non-critical: the selection just will not survive a reload.
      }
      setUnreadCounts((prev) => ({ ...prev, [next]: 0 }));
    },
    [activeChannelId, leaveGroup, joinGroup]
  );

  // Rejects on failure so the composer can restore the user's draft.
  const handleSendMessage = useCallback(
    async (text: string) => {
      if (isOnboardingRequired) return;
      if (activeChannelId === DEFAULT_CHANNEL_ID) {
        await sendMessage(currentUser, text);
      } else {
        await sendMessageToGroup(activeChannelId, currentUser, text);
      }
    },
    [isOnboardingRequired, activeChannelId, currentUser, sendMessage, sendMessageToGroup]
  );

  return (
    <>
      <ChatLayout
        channels={channelsWithUnread}
        activeChannel={activeChannel}
        messages={activeMessages}
        currentUser={currentUser}
        connectionStatus={connectionState}
        onSelectChannel={handleSelectChannel}
        onSendMessage={handleSendMessage}
        onEditProfile={() => setIsProfileOpen(true)}
      />
      <OnboardingModal isOpen={isOnboardingRequired} onSubmit={saveUsername} />
      <ProfileModal
        isOpen={isProfileOpen && !isOnboardingRequired}
        currentUsername={currentUser}
        onSave={handleProfileSave}
        onClose={() => setIsProfileOpen(false)}
      />
    </>
  );
}

export default App;
