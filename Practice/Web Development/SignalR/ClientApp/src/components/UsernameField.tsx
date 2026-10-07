import { useId } from 'react';
import { validateUsername } from '@/types/chat';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';

interface UsernameFieldProps {
  readonly label: string;
  readonly value: string;
  readonly touched: boolean;
  readonly onChange: (value: string) => void;
  readonly onBlur: () => void;
  readonly placeholder?: string;
}

/** Labeled username input with inline validation, shared by the onboarding and profile dialogs. */
export const UsernameField = ({ label, value, touched, onChange, onBlur, placeholder }: UsernameFieldProps) => {
  const inputId = useId();
  const helpId = useId();
  const validation = validateUsername(value);
  const showError = touched && !validation.isValid;

  return (
    <div className="grid gap-2">
      <Label htmlFor={inputId}>{label}</Label>
      <Input
        id={inputId}
        value={value}
        onChange={(event) => onChange(event.target.value)}
        onBlur={onBlur}
        placeholder={placeholder}
        maxLength={30}
        autoComplete="off"
        autoFocus
        aria-invalid={showError}
        aria-describedby={helpId}
        className={showError ? 'border-destructive focus-visible:ring-destructive' : undefined}
      />
      <p id={helpId} className={showError ? 'text-xs text-destructive' : 'text-xs text-muted-foreground'}>
        {showError ? validation.error : '2 to 25 characters. Letters, numbers, spaces, hyphens and underscores.'}
      </p>
    </div>
  );
};

