import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../../environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TotalSales } from './total-sales.model';
import { TotalSalesRequest } from './total-sales-request.model';

@Injectable({
  providedIn: 'root',
})
export class TotalSalesService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getTotalSales(request: TotalSalesRequest): Observable<TotalSales> {
    const params = new HttpParams({
      fromObject: request as any,
    });

    return this.httpClient.get<TotalSales>(`${this.environment.apiUrl}/analytics/total-sales`, { params });
  }
}
