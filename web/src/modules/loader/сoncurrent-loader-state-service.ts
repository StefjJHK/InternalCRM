import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, pipe, UnaryFunction } from 'rxjs';
import { distinctUntilChanged, finalize, map } from 'rxjs/operators';

@Injectable()
export class ConcurrentLoaderStateService {
  public get isLoading(): boolean {
    return this.isLoadingSource.value > 0;
  }

  public get isLoading$(): Observable<boolean> {
    return this.isLoadingSource.pipe(
      map((value: number) => value > 0),
      distinctUntilChanged(),
    );
  }

  private isLoadingSource: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  public pushLoadingState(isLoading: boolean): void {
    const currentCounter: number = this.isLoadingSource.value;
    const nextCounter: number = Math.max(currentCounter + (isLoading ? 1 : -1), 0);

    this.isLoadingSource.next(nextCounter);
  }

  public wrap<T>(observable$: Observable<T>): Observable<T> {
    this.pushLoadingState(true);

    return observable$.pipe(
      finalize(() => {
        this.pushLoadingState(false);
      }),
    );
  }

  public wrapPipe<T>(): UnaryFunction<Observable<T>, Observable<T>> {
    return pipe((o$: Observable<T>) => this.wrap(o$));
  }
}
