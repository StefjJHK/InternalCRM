import { Observable, of, OperatorFunction, pipe, throwError } from 'rxjs';
import { Predicate } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

export interface ErrorHandler {
  match: (error: unknown) => boolean;
  handle: (error: any) => unknown; //eslint-disable-line @typescript-eslint/no-explicit-any
}

export interface ErrorConstructor<T extends Error> {
  new (...args: any[]): T; //eslint-disable-line @typescript-eslint/no-explicit-any
  readonly prototype: T;
}

export interface HttpErrorMatchOptions {
  statusCode: HttpStatusCode;
}

//eslint-disable-next-line @typescript-eslint/no-explicit-any
export function handleErrors(errorHandlers: ErrorHandler[]): OperatorFunction<any, any> {
  return pipe(
    catchError((err) => {
      const handler = errorHandlers.find((_: ErrorHandler) => _.match(err));
      if (handler) {
        const result = handler.handle(err);
        if (result instanceof Observable) {
          return result;
        } else if (result instanceof Error) {
          return throwError(() => result);
        } else if (result === undefined) {
          return throwError(() => err);
        } else {
          return of(result);
        }
      }
      return throwError(() => err);
    }),
  );
}

export function errorHandler<TError extends Error, TOutput = unknown>(
  handle: (error: TError) => TOutput,
  match?: Predicate<TError>,
): ErrorHandler {
  return {
    handle: handle,
    match: (error: unknown) => {
      if (match == null) {
        return true;
      }

      return match(error as TError);
    },
  };
}

export function errorHandlerByType<TError extends Error, TOutput = unknown>(
  handle: (error: TError) => TOutput,
  match: ErrorConstructor<TError>,
): ErrorHandler {
  return {
    handle: handle,
    match: (error: unknown) => error instanceof match,
  };
}

export function errorHandlerByHttp<TError extends HttpErrorResponse, TOutput = unknown>(
  handle: (error: TError) => TOutput,
  match: HttpErrorMatchOptions,
): ErrorHandler {
  return {
    handle: handle,
    match: (error: unknown) => {
      if (error instanceof HttpErrorResponse) {
        if (error.status !== match.statusCode) {
          return false;
        }

        return true;
      }
      return false;
    },
  };
}
