import { memo } from 'react';
import type { ChatMessage } from '@/types/chat';
import { cn } from '@/lib/utils';
import { formatTime, getAvatarTint, getInitials } from '@/utils/format';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';

interface MessageItemProps {
  readonly message: ChatMessage;
  readonly isOwn: boolean;
  /** When false the row continues a run from the same sender and omits avatar and header. */
  readonly showHeader: boolean;
}

export const MessageItem = memo(({ message, isOwn, showHeader }: MessageItemProps) => {
  const time = formatTime(message.timestamp);

  return (
    <article
      className={cn(
        'flex gap-2.5 px-4 sm:px-6',
        isOwn ? 'flex-row-reverse' : 'flex-row',
        showHeader ? 'mt-4' : 'mt-0.5'
      )}
    >
      <div className="w-8 shrink-0">
        {showHeader && !isOwn && (
          <Avatar className="size-8">
            <AvatarFallback className={cn('text-xs font-medium', getAvatarTint(message.user))}>
              {getInitials(message.user)}
            </AvatarFallback>
          </Avatar>
        )}
      </div>

      <div className={cn('flex min-w-0 max-w-[75%] flex-col', isOwn ? 'items-end' : 'items-start')}>
        {showHeader && (
          <header className="mb-1 flex items-baseline gap-2 px-1">
            <span className="text-sm font-medium text-foreground">{isOwn ? 'You' : message.user}</span>
            {time && <time dateTime={message.timestamp} className="text-xs text-muted-foreground">{time}</time>}
          </header>
        )}
        <p
          className={cn(
            'whitespace-pre-wrap break-words rounded-2xl px-3.5 py-2 text-sm leading-relaxed [overflow-wrap:anywhere]',
            isOwn
              ? 'rounded-tr-md bg-primary text-primary-foreground'
              : 'rounded-tl-md bg-muted text-foreground'
          )}
        >
          {message.message}
        </p>
      </div>
    </article>
  );
});

MessageItem.displayName = 'MessageItem';

