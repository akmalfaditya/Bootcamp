import { Fragment, useLayoutEffect, useMemo, useRef } from 'react';
import type { ChatMessage, Channel } from '@/types/chat';
import { formatDayLabel, getDayKey } from '@/utils/format';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Separator } from '@/components/ui/separator';
import { MessageItem } from '@/components/chat/MessageItem';

interface MessageListProps {
  readonly messages: readonly ChatMessage[];
  readonly currentUser: string;
  readonly channel: Channel;
}

/** Messages from the same sender within this window share one header. */
const GROUP_WINDOW_MS = 5 * 60 * 1000;
/** Distance from the bottom in pixels within which the feed keeps following new messages. */
const FOLLOW_THRESHOLD_PX = 120;

interface FeedRow {
  readonly message: ChatMessage;
  readonly showHeader: boolean;
  readonly dayLabel: string | null;
}

function buildRows(messages: readonly ChatMessage[]): FeedRow[] {
  return messages.map((message, index) => {
    const previous = index > 0 ? messages[index - 1] : null;
    const isNewDay = !previous || getDayKey(previous.timestamp) !== getDayKey(message.timestamp);
    const gap = previous ? Date.parse(message.timestamp) - Date.parse(previous.timestamp) : Infinity;
    const showHeader = isNewDay || previous?.user !== message.user || gap > GROUP_WINDOW_MS;
    return { message, showHeader, dayLabel: isNewDay ? formatDayLabel(message.timestamp) : null };
  });
}

export const MessageList = ({ messages, currentUser, channel }: MessageListProps) => {
  const viewportRef = useRef<HTMLDivElement>(null);
  const bottomAnchorRef = useRef<HTMLDivElement>(null);
  const isNearBottomRef = useRef(true);

  const rows = useMemo(() => buildRows(messages), [messages]);
  const lastMessage = messages.length > 0 ? messages[messages.length - 1] : null;

  const handleScroll = (event: React.UIEvent<HTMLDivElement>) => {
    const el = event.currentTarget;
    const distanceFromBottom = el.scrollHeight - el.scrollTop - el.clientHeight;
    isNearBottomRef.current = distanceFromBottom <= FOLLOW_THRESHOLD_PX;
  };

  // Follow new messages when the reader is at the bottom, and always after the local user sends.
  useLayoutEffect(() => {
    if (!lastMessage) return;
    const isSentByMe = lastMessage.user === currentUser;
    if (isNearBottomRef.current || isSentByMe) {
      bottomAnchorRef.current?.scrollIntoView({ behavior: 'smooth' });
      isNearBottomRef.current = true;
    }
  }, [lastMessage, currentUser]);

  return (
    <div
      role="log"
      aria-live="polite"
      aria-label={`Messages in ${channel.name}`}
      className="relative flex-1 min-h-0 h-full overflow-hidden"
    >
      <ScrollArea
        viewportRef={viewportRef}
        onScroll={handleScroll}
        className="h-full w-full"
      >
        <div className="flex flex-col min-w-0 w-full py-4">
          {rows.length === 0 ? (
            <div className="flex h-64 flex-col items-center justify-center px-6 text-center">
              <p className="text-sm font-medium text-foreground"># {channel.name}</p>
              <p className="mt-1 max-w-xs text-sm text-muted-foreground">
                No messages yet. Messages sent here are visible to everyone in this channel.
              </p>
            </div>
          ) : (
            rows.map(({ message, showHeader, dayLabel }) => (
              <Fragment key={message.id}>
                {dayLabel && (
                  <div className="my-4 flex items-center gap-3 px-4 sm:px-6" role="separator" aria-label={dayLabel}>
                    <Separator className="flex-1" />
                    <span className="text-xs font-medium text-muted-foreground">{dayLabel}</span>
                    <Separator className="flex-1" />
                  </div>
                )}
                <MessageItem
                  message={message}
                  isOwn={message.user === currentUser}
                  showHeader={showHeader}
                />
              </Fragment>
            ))
          )}
          <div ref={bottomAnchorRef} aria-hidden="true" className="h-px w-full" />
        </div>
      </ScrollArea>
    </div>
  );
};
