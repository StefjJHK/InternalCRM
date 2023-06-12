import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../../environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TotalCustomersRequest } from './total-customers-request.model';
import { TotalCustomers } from './total-customers.model';

@Injectable({
  providedIn: 'root',
})
export class TotalCustomersService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getTotalCustomers(request: TotalCustomersRequest): Observable<TotalCustomers> {
    const params = new HttpParams({
      fromObject: request as any,
    });

    return this.httpClient.get<TotalCustomers>(`${this.environment.apiUrl}/analytics/total-customers`, { params });
  }
}
