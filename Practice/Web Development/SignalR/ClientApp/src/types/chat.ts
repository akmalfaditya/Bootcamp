/**
 * Shared TypeScript Contracts & Data Models for Enterprise SignalR Chat
 * Target Location: ClientApp/src/types/chat.ts
 */

// ============================================================================
// 1. Connection & Lifecycle Models
// ============================================================================

/**
 * Normalized client connection status mirroring SignalR Hub lifecycle
 */
export type ConnectionStatus = 'Connected' | 'Reconnecting' | 'Disconnected';

// ============================================================================
// 2. Chat Domain Models
// ============================================================================

/**
 * Represents a single chat message rendered in the chat stream
 */
export interface ChatMessage {
  /** Unique message identifier (UUID or stable timestamp string) */
  readonly id: string;
  /** Display handle of the message sender */
  readonly user: string;
  /** Content of the message */
  readonly message: string;
  /** Normalized channel name (e.g., 'general', 'engineering', 'announcements') */
  readonly channel: string;
  /** ISO 8601 string or formatted time string */
  readonly timestamp: string;
  /** Indicates whether the message was dispatched via external REST API */
  readonly isRestApi?: boolean;
}

/**
 * Represents a chat channel / room
 */
export interface Channel {
  /** Normalized channel identifier without leading hash (e.g., 'general') */
  readonly id: string;
  /** Display name of the channel (e.g., 'general' or '#general') */
  readonly name: string;
  /** Description / topic for channel header */
  readonly description: string;
  /** Count of unread messages when user is focused on another channel */
  readonly unreadCount?: number;
}

/**
 * Current user profile and session identity
 */
export interface UserProfile {
  /** Active user display handle */
  readonly username: string;
  /** Timestamp when user onboarded / joined */
  readonly joinedAt?: string;
}

// ============================================================================
// 3. Hub & Wire Protocol Contracts
// ============================================================================

/**
 * Methods callable by the client on the backend SignalR ChatHub
 */
export interface ChatHubServerMethods {
  /** Broadcasts message to all connected clients via Clients.All.ReceiveMessage */
  SendMessage: (user: string, message: string) => Promise<void>;
  /** Subscribes connection to a named group via Groups.AddToGroupAsync */
  JoinGroup: (groupName: string) => Promise<void>;
  /** Unsubscribes connection from a named group via Groups.RemoveFromGroupAsync */
  LeaveGroup: (groupName: string) => Promise<void>;
  /** Broadcasts message to group members via Clients.Group.ReceiveGroupMessage */
  SendMessageToGroup: (groupName: string, user: string, message: string) => Promise<void>;
}

/**
 * Events pushed from the backend SignalR ChatHub to the client
 */
export interface ChatHubClientEvents {
  /** Global broadcast event listener */
  ReceiveMessage: (user: string, message: string) => void;
  /** Group-isolated message event listener */
  ReceiveGroupMessage: (groupName: string, user: string, message: string) => void;
}

/**
 * Wire DTO for REST API endpoints (POST /api/message and POST /api/message/group/{groupName})
 */
export interface MessageDto {
  readonly user: string;
  readonly message: string;
}

// ============================================================================
// 4. Hook Contracts (`useSignalR`)
// ============================================================================

export interface UseSignalROptions {
  /** Relative or absolute URL to SignalR Hub endpoint (defaults to '/hubs/chat') */
  readonly url?: string;
  /** Active channel group for automatic subscription / reconnect resubscription */
  readonly activeChannel?: string;
  /** Default channel name (defaults to 'general') */
  readonly defaultChannel?: string;
  /** Auto-connect on mount (defaults to true) */
  readonly autoConnect?: boolean;
  /** Optional custom predicate to determine if a message came from a REST API */
  readonly isRestApiPredicate?: (user: string, message: string) => boolean;
}

export interface UseSignalRReturn {
  /** Current connection lifecycle status (aliased for connectionStatus and connectionState) */
  readonly connectionStatus: ConnectionStatus;
  readonly connectionState: ConnectionStatus;
  /** Array of received chat messages */
  readonly messages: readonly ChatMessage[];
  /** Subscribed channel groups */
  readonly subscribedGroups: readonly string[];
  /** Error message string or null */
  readonly error: string | null;
  /** Boolean shorthand for connectionStatus === 'Connected' */
  readonly isConnected: boolean;
  /** Dispatch global broadcast message */
  readonly sendMessage: (user: string, message: string) => Promise<void>;
  /** Join a named channel group */
  readonly joinGroup: (groupName: string) => Promise<void>;
  /** Leave a named channel group */
  readonly leaveGroup: (groupName: string) => Promise<void>;
  /** Dispatch message to a specific channel group */
  readonly sendMessageToGroup: (groupName: string, user: string, message: string) => Promise<void>;
  /** Clear local message stream */
  readonly clearMessages: () => void;
  /** Clear active error state */
  readonly clearError: () => void;
  /** Reconnect manually */
  readonly reconnect: () => Promise<void>;
}

// ============================================================================
// 6. Validation Models & Utility Types
// ============================================================================

export interface UsernameValidationResult {
  readonly isValid: boolean;
  readonly error?: string;
}

export type DefaultChannelId = 'general' | 'engineering' | 'announcements';

// ============================================================================
// 7. Standard System Constants & Helpers
// ============================================================================

/**
 * Standard enterprise channel roster
 */
export const DEFAULT_CHANNELS: readonly Channel[] = [
  {
    id: 'general',
    name: 'general',
    description: 'General discussion and company announcements',
    unreadCount: 0,
  },
  {
    id: 'engineering',
    name: 'engineering',
    description: 'Technical architecture, code reviews, and development discussions',
    unreadCount: 0,
  },
  {
    id: 'announcements',
    name: 'announcements',
    description: 'Official enterprise bulletins and critical broadcast alerts',
    unreadCount: 0,
  },
] as const;

export const DEFAULT_CHANNEL_ID: DefaultChannelId = 'general';

/**
 * LocalStorage keys for session persistence
 */
export const STORAGE_KEYS = {
  USERNAME: 'signalr_enterprise_user',
  ACTIVE_CHANNEL: 'signalr_enterprise_active_channel',
} as const;

/**
 * Normalizes channel strings by removing leading hashes, trimming, and lowercasing
 */
export function normalizeChannelId(channel: string): string {
  if (!channel) return '';
  return channel.trim().replace(/^#+/, '').toLowerCase();
}

/**
 * Validates display handle per Enterprise UX requirements (2–25 chars, alphanumeric/spaces)
 */
export function validateUsername(username: string): UsernameValidationResult {
  const trimmed = username.trim();
  if (!trimmed) {
    return { isValid: false, error: 'Username is required.' };
  }
  if (trimmed.length < 2) {
    return { isValid: false, error: 'Username must be at least 2 characters.' };
  }
  if (trimmed.length > 25) {
    return { isValid: false, error: 'Username cannot exceed 25 characters.' };
  }
  // Allow alphanumeric, underscores, hyphens, and single spaces between words
  const validPattern = /^[a-zA-Z0-9_\-]+( [a-zA-Z0-9_\-]+)*$/;
  if (!validPattern.test(trimmed)) {
    return {
      isValid: false,
      error: 'Username can only contain alphanumeric characters, underscores, hyphens, and spaces.',
    };
  }
  return { isValid: true };
}

/**
 * Helper factory to instantiate a ChatMessage with defaults
 */
export function createChatMessage(params: {
  user: string;
  message: string;
  channel?: string;
  isRestApi?: boolean;
  id?: string;
  timestamp?: string;
}): ChatMessage {
  const isApi = params.isRestApi ?? (
    params.user.toLowerCase().includes('bot') ||
    params.user.toLowerCase().includes('admin') ||
    params.user.toLowerCase().includes('system') ||
    params.user.toLowerCase().includes('rest') ||
    params.user.toLowerCase().includes('api')
  );

  return {
    id: params.id ?? (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function' ? crypto.randomUUID() : `${Date.now()}-${Math.random().toString(36).substring(2, 9)}`),
    user: params.user,
    message: params.message,
    channel: params.channel ? normalizeChannelId(params.channel) : DEFAULT_CHANNEL_ID,
    timestamp: params.timestamp ?? new Date().toISOString(),
    isRestApi: isApi,
  };
}
