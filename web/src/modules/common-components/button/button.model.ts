export interface Button {
  text: string;
  type: 'primary' | 'default' | 'danger' | 'text';
  icon?: string;

  onClick(): void;
}
