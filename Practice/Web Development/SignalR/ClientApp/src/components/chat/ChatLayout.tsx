import { useCallback, useState } from 'react';
import type { Channel, ChatMessage, ConnectionStatus } from '@/types/chat';
import { Sheet, SheetContent, SheetTitle } from '@/components/ui/sheet';
import { TooltipProvider } from '@/components/ui/tooltip';
import { ChatHeader } from '@/components/chat/ChatHeader';
import { ChatInput } from '@/components/chat/ChatInput';
import { ChatSidebar } from '@/components/chat/ChatSidebar';
import { MessageList } from '@/components/chat/MessageList';

interface ChatLayoutProps {
  readonly channels: readonly Channel[];
  readonly activeChannel: Channel;
  readonly messages: readonly ChatMessage[];
  readonly currentUser: string;
  readonly connectionStatus: ConnectionStatus;
  readonly onSelectChannel: (channelId: string) => void;
  readonly onSendMessage: (text: string) => Promise<void>;
  readonly onEditProfile: () => void;
}

export const ChatLayout = ({
  channels,
  activeChannel,
  messages,
  currentUser,
  connectionStatus,
  onSelectChannel,
  onSendMessage,
  onEditProfile,
}: ChatLayoutProps) => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);

  const handleSelectChannel = useCallback(
    (channelId: string) => {
      onSelectChannel(channelId);
      setIsDrawerOpen(false);
    },
    [onSelectChannel]
  );

  const sidebar = (
    <ChatSidebar
      channels={channels}
      activeChannelId={activeChannel.id}
      username={currentUser || 'Guest'}
      connectionStatus={connectionStatus}
      onSelectChannel={handleSelectChannel}
      onEditProfile={onEditProfile}
    />
  );

  return (
    <TooltipProvider delayDuration={200}>
      <div className="flex h-screen h-dvh w-full overflow-hidden bg-background">
        <aside className="hidden w-64 shrink-0 border-r md:block">{sidebar}</aside>

        <Sheet open={isDrawerOpen} onOpenChange={setIsDrawerOpen}>
          <SheetContent side="left" className="w-72 p-0 md:hidden" aria-describedby={undefined}>
            <SheetTitle className="sr-only">Channels</SheetTitle>
            {sidebar}
          </SheetContent>
        </Sheet>

        <main className="flex min-w-0 min-h-0 flex-1 flex-col overflow-hidden">
          <ChatHeader
            channel={activeChannel}
            connectionStatus={connectionStatus}
            onOpenSidebar={() => setIsDrawerOpen(true)}
          />
          <MessageList
            key={activeChannel.id}
            messages={messages}
            currentUser={currentUser}
            channel={activeChannel}
          />
          <ChatInput
            channelName={activeChannel.name}
            disabled={connectionStatus !== 'Connected' || !currentUser}
            onSend={onSendMessage}
          />
        </main>
      </div>
    </TooltipProvider>
  );
};
