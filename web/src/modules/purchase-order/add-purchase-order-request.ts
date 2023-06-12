export interface AddPurchaseOrderRequest {
  customerName: string;
  productName: string;
  number: string;
  amount: number;
  receivedDate: string;
  dueDate: string;
}
