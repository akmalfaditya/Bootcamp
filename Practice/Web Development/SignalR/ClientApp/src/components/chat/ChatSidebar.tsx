import { Hash, Pencil, Users } from 'lucide-react';
import type { Channel, ConnectionStatus } from '@/types/chat';
import { cn } from '@/lib/utils';
import { getAvatarTint, getInitials } from '@/utils/format';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Separator } from '@/components/ui/separator';
import { Tooltip, TooltipContent, TooltipTrigger } from '@/components/ui/tooltip';
import { ConnectionDot } from '@/components/chat/ConnectionDot';

interface ChatSidebarProps {
  readonly channels: readonly Channel[];
  readonly activeChannelId: string;
  readonly username: string;
  readonly connectionStatus: ConnectionStatus;
  readonly onSelectChannel: (channelId: string) => void;
  readonly onEditProfile: () => void;
}

export const ChatSidebar = ({
  channels,
  activeChannelId,
  username,
  connectionStatus,
  onSelectChannel,
  onEditProfile,
}: ChatSidebarProps) => (
  <div className="flex h-full min-h-0 flex-col bg-muted/40">
    <div className="flex h-14 shrink-0 items-center justify-between px-4">
      <span className="text-sm font-semibold tracking-tight">Team Workspace</span>
    </div>
    <Separator />

    <ScrollArea className="min-h-0 flex-1">
      <div className="flex flex-col gap-4 p-2">
        {/* Channels Section */}
        <nav aria-label="Channels" className="flex flex-col gap-0.5">
          <p className="px-2 pb-1 pt-1 text-xs font-medium uppercase tracking-wider text-muted-foreground">
            Channels
          </p>
          {channels.map((channel) => {
            const isActive = channel.id === activeChannelId;
            const unread = channel.unreadCount ?? 0;
            return (
              <Button
                key={channel.id}
                type="button"
                variant="ghost"
                aria-current={isActive ? 'page' : undefined}
                onClick={() => onSelectChannel(channel.id)}
                className={cn(
                  'h-8 justify-start gap-2 px-2 font-normal',
                  isActive
                    ? 'bg-accent font-medium text-accent-foreground'
                    : 'text-muted-foreground hover:text-foreground'
                )}
              >
                <Hash className="size-4 shrink-0 text-muted-foreground" />
                <span className="truncate">{channel.name}</span>
                {unread > 0 && (
                  <Badge className="ml-auto h-5 min-w-5 justify-center px-1.5 text-[11px]">
                    <span aria-hidden="true">{unread > 99 ? '99+' : unread}</span>
                    <span className="sr-only">{unread} unread messages</span>
                  </Badge>
                )}
              </Button>
            );
          })}
        </nav>

        {/* Online Presence / Participants Section */}
        <div className="flex flex-col gap-0.5">
          <div className="flex items-center justify-between px-2 pb-1 pt-1">
            <p className="text-xs font-medium uppercase tracking-wider text-muted-foreground">
              Online Members
            </p>
            <Users className="size-3.5 text-muted-foreground" />
          </div>
          <Button
            type="button"
            variant="ghost"
            className="h-8 justify-start gap-2 px-2 font-normal text-foreground"
          >
            <div className="relative">
              <Avatar className="size-5">
                <AvatarFallback className={cn('text-[10px] font-medium', getAvatarTint(username))}>
                  {getInitials(username)}
                </AvatarFallback>
              </Avatar>
              <span
                className={cn(
                  'absolute -bottom-0.5 -right-0.5 size-2 rounded-full ring-1 ring-background',
                  connectionStatus === 'Connected'
                    ? 'bg-emerald-500'
                    : connectionStatus === 'Reconnecting'
                    ? 'bg-amber-500 animate-pulse'
                    : 'bg-zinc-400'
                )}
              />
            </div>
            <span className="truncate text-xs">{username} (you)</span>
          </Button>
        </div>
      </div>
    </ScrollArea>

    <Separator />

    {/* Profile Footer */}
    <div className="flex shrink-0 items-center gap-2.5 p-3">
      <Avatar className="size-8">
        <AvatarFallback className={cn('text-xs font-medium', getAvatarTint(username))}>
          {getInitials(username)}
        </AvatarFallback>
      </Avatar>
      <div className="min-w-0 flex-1">
        <p className="truncate text-sm font-medium leading-tight">{username}</p>
        <ConnectionDot status={connectionStatus} />
      </div>
      <Tooltip>
        <TooltipTrigger asChild>
          <Button
            type="button"
            variant="ghost"
            size="icon"
            className="size-8"
            onClick={onEditProfile}
            aria-label="Edit display name"
          >
            <Pencil className="size-4" />
          </Button>
        </TooltipTrigger>
        <TooltipContent side="top">Edit display name</TooltipContent>
      </Tooltip>
    </div>
  </div>
);
