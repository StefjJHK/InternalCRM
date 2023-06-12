import { ModalActionRoles } from './modal-action-roles';

export interface ModalAction {
  id: string;
  title: string;
  type: 'primary' | 'default';
  role: ModalActionRoles;

  onClick(): void;
  disabled?(): boolean;
}
