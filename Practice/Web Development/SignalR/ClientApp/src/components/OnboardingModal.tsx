import { useState, type FormEvent } from 'react';
import { validateUsername } from '@/types/chat';
import { Button } from '@/components/ui/button';
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { UsernameField } from '@/components/UsernameField';

interface OnboardingModalProps {
  readonly isOpen: boolean;
  readonly onSubmit: (username: string) => void;
}

interface OnboardingFormProps {
  readonly onSubmit: (username: string) => void;
}

// Mounted only while the dialog is open, so its state starts fresh without an effect.
const OnboardingForm = ({ onSubmit }: OnboardingFormProps) => {
  const [username, setUsername] = useState('');
  const [touched, setTouched] = useState(false);
  const isValid = validateUsername(username).isValid;

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setTouched(true);
    if (isValid) onSubmit(username.trim());
  };

  return (
    <form onSubmit={handleSubmit} className="grid gap-4" noValidate>
      <UsernameField
        label="Display name"
        value={username}
        touched={touched}
        onChange={(value) => {
          setUsername(value);
          setTouched(true);
        }}
        onBlur={() => setTouched(true)}
        placeholder="e.g. Sarah Connor"
      />
      <Button type="submit" disabled={!isValid}>
        Continue
      </Button>
    </form>
  );
};

/** Blocking dialog: it cannot be dismissed until a valid display name is submitted. */
export const OnboardingModal = ({ isOpen, onSubmit }: OnboardingModalProps) => (
  <Dialog open={isOpen}>
    <DialogContent
      hideClose
      className="max-w-sm"
      onEscapeKeyDown={(event) => event.preventDefault()}
      onPointerDownOutside={(event) => event.preventDefault()}
      onInteractOutside={(event) => event.preventDefault()}
    >
      <DialogHeader>
        <DialogTitle>Choose a display name</DialogTitle>
        <DialogDescription>This is how other people in the channels will see you.</DialogDescription>
      </DialogHeader>
      <OnboardingForm onSubmit={onSubmit} />
    </DialogContent>
  </Dialog>
);
