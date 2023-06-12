export interface UpdateInvoiceRequest {
  number?: string;
  amount?: number;
  receivedDate?: string;
  dueDate?: string;
  purchaseOrderNumber?: string;
  productName?: string;
  customerName?: string;
}
