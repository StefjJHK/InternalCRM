export interface Subscription {
  number: string;
  subLegalEntity: string;
  cost: number;
  isPaid: true;
  validFrom: Date;
  validUntil: Date;
}
