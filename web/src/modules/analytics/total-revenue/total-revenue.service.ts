import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../../environment';
import { TotalRevenueRequest } from './total-revenue-request.model';
import { TotalRevenue } from './total-revenue.model';

@Injectable({
  providedIn: 'root',
})
export class TotalRevenueService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getTotalRevenue(request: TotalRevenueRequest): Observable<TotalRevenue> {
    const params = new HttpParams({
      fromObject: request as any,
    });

    return this.httpClient.get<TotalRevenue>(`${this.environment.apiUrl}/analytics/total-revenue`, { params });
  }
}
