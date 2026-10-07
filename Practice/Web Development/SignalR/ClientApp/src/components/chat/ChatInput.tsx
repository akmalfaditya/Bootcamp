import { useRef, useState, type FormEvent, type KeyboardEvent } from 'react';
import { SendHorizontal } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';

interface ChatInputProps {
  readonly channelName: string;
  readonly disabled: boolean;
  /** Should reject on failure so the draft can be restored. */
  readonly onSend: (text: string) => Promise<void>;
}

export const ChatInput = ({ channelName, disabled, onSend }: ChatInputProps) => {
  const [text, setText] = useState('');
  const textareaRef = useRef<HTMLTextAreaElement>(null);
  const canSend = !disabled && text.trim().length > 0;

  const dispatchMessage = async () => {
    const trimmed = text.trim();
    if (disabled || !trimmed) return;

    setText('');
    try {
      await onSend(trimmed);
    } catch {
      // Restore the draft unless the user has already typed something new.
      setText((current) => (current === '' ? trimmed : current));
    } finally {
      textareaRef.current?.focus();
    }
  };

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    await dispatchMessage();
  };

  const handleKeyDown = (event: KeyboardEvent<HTMLTextAreaElement>) => {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      void dispatchMessage();
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex items-end gap-2 border-t bg-background px-4 py-3 sm:px-6">
      <Textarea
        ref={textareaRef}
        value={text}
        onChange={(event) => setText(event.target.value)}
        onKeyDown={handleKeyDown}
        disabled={disabled}
        placeholder={disabled ? 'Waiting for connection...' : `Message #${channelName}`}
        aria-label={`Message #${channelName}`}
        autoFocus
        rows={1}
        className="min-h-[42px] max-h-32 resize-none py-2.5 text-sm"
      />
      <Button
        type="submit"
        size="icon"
        disabled={!canSend}
        aria-label="Send message"
        className="size-10 shrink-0"
      >
        <SendHorizontal className="size-4" />
      </Button>
    </form>
  );
};
