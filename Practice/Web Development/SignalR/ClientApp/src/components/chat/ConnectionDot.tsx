import type { ConnectionStatus } from '@/types/chat';
import { cn } from '@/lib/utils';
import { Tooltip, TooltipContent, TooltipTrigger } from '@/components/ui/tooltip';

interface ConnectionDotProps {
  readonly status: ConnectionStatus;
  readonly showLabel?: boolean;
  readonly className?: string;
}

const STATUS_STYLES: Record<ConnectionStatus, { dot: string; label: string }> = {
  Connected: { dot: 'bg-emerald-500', label: 'Connected' },
  Reconnecting: { dot: 'bg-amber-500 animate-pulse', label: 'Reconnecting...' },
  Disconnected: { dot: 'bg-zinc-400', label: 'Disconnected' },
};

/** Small status dot; the label is available as text for screen readers and as a tooltip. */
export const ConnectionDot = ({ status, showLabel = false, className }: ConnectionDotProps) => {
  const { dot, label } = STATUS_STYLES[status];

  return (
    <Tooltip>
      <TooltipTrigger asChild>
        <span
          role="status"
          aria-label={label}
          className={cn('inline-flex items-center gap-1.5 text-xs text-muted-foreground', className)}
        >
          <span className={cn('size-2 rounded-full', dot)} aria-hidden="true" />
          {showLabel ? <span>{label}</span> : <span className="sr-only">{label}</span>}
        </span>
      </TooltipTrigger>
      <TooltipContent side="bottom">{label}</TooltipContent>
    </Tooltip>
  );
};

