import { Injectable } from '@angular/core';
import { ConcurrentLoaderStateService } from '../сoncurrent-loader-state-service';

@Injectable({
  providedIn: 'root',
})
export class GlobalLoaderService extends ConcurrentLoaderStateService {}
