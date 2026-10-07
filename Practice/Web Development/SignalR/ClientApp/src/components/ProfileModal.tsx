import { useState, type FormEvent } from 'react';
import { validateUsername } from '@/types/chat';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { UsernameField } from '@/components/UsernameField';

interface ProfileModalProps {
  readonly isOpen: boolean;
  readonly currentUsername: string;
  readonly onSave: (username: string) => void;
  readonly onClose: () => void;
}

interface ProfileFormProps {
  readonly currentUsername: string;
  readonly onSave: (username: string) => void;
  readonly onClose: () => void;
}

// Mounted only while the dialog is open, so the draft always starts from the current name.
const ProfileForm = ({ currentUsername, onSave, onClose }: ProfileFormProps) => {
  const [username, setUsername] = useState(currentUsername);
  const [touched, setTouched] = useState(false);
  const trimmed = username.trim();
  const canSave = validateUsername(username).isValid && trimmed !== currentUsername.trim();

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setTouched(true);
    if (canSave) onSave(trimmed);
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
      />
      <DialogFooter className="gap-2 sm:gap-0">
        <Button type="button" variant="outline" onClick={onClose}>
          Cancel
        </Button>
        <Button type="submit" disabled={!canSave}>
          Save
        </Button>
      </DialogFooter>
    </form>
  );
};

export const ProfileModal = ({ isOpen, currentUsername, onSave, onClose }: ProfileModalProps) => (
  <Dialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
    <DialogContent className="max-w-sm">
      <DialogHeader>
        <DialogTitle>Edit display name</DialogTitle>
        <DialogDescription>Your new name applies to messages you send from now on.</DialogDescription>
      </DialogHeader>
      <ProfileForm currentUsername={currentUsername} onSave={onSave} onClose={onClose} />
    </DialogContent>
  </Dialog>
);
