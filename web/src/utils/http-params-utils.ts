import { HttpParams } from '@angular/common/http';

export function createQueryParams(obj: object): HttpParams {
  return new HttpParams({ fromObject: JSON.parse(JSON.stringify(obj)) });
}
