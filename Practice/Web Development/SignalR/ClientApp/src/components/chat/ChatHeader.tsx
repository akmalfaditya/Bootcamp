import { Hash, Menu, Users } from 'lucide-react';
import type { Channel, ConnectionStatus } from '@/types/chat';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { Tooltip, TooltipContent, TooltipTrigger } from '@/components/ui/tooltip';
import { ConnectionDot } from '@/components/chat/ConnectionDot';

interface ChatHeaderProps {
  readonly channel: Channel;
  readonly connectionStatus: ConnectionStatus;
  readonly onOpenSidebar: () => void;
}

export const ChatHeader = ({ channel, connectionStatus, onOpenSidebar }: ChatHeaderProps) => (
  <header className="flex h-14 shrink-0 items-center gap-3 border-b bg-background px-4 sm:px-6">
    <Button
      type="button"
      variant="ghost"
      size="icon"
      className="-ml-2 md:hidden"
      onClick={onOpenSidebar}
      aria-label="Open channels"
    >
      <Menu className="size-5" />
    </Button>

    <div className="flex items-center gap-1.5 font-semibold text-sm">
      <Hash className="size-4 text-muted-foreground shrink-0" />
      <h1 className="truncate">{channel.name}</h1>
    </div>

    <Separator orientation="vertical" className="hidden h-4 sm:block" />
    <p className="hidden min-w-0 flex-1 truncate text-xs text-muted-foreground sm:block">
      {channel.description}
    </p>

    <div className="ml-auto flex items-center gap-3">
      <Tooltip>
        <TooltipTrigger asChild>
          <div className="hidden items-center gap-1.5 text-xs text-muted-foreground sm:flex">
            <Users className="size-3.5" />
            <span>Channel active</span>
          </div>
        </TooltipTrigger>
        <TooltipContent side="bottom">Participants in #{channel.name}</TooltipContent>
      </Tooltip>

      <Separator orientation="vertical" className="hidden h-4 sm:block" />

      <ConnectionDot status={connectionStatus} showLabel />
    </div>
  </header>
);
