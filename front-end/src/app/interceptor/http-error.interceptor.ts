import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Si è verificato un errore sconosciuto.';
        const statusCode = error.status;

        if (error.error && error.error.errors) {
          errorMessage = this.formatValidationErrors(error.error.errors);
        } else if (error.error instanceof ErrorEvent) {
          // errore lato client
          errorMessage = `Errore: ${error.error.message}`;
        } else {
          errorMessage = error.error?.message || errorMessage;
          // errori lato server
          // personalizzazione messaggio di errore in base al codice di stato
          switch (statusCode) {
            case 400: {
              errorMessage = error.error?.message || 'Richiesta errata';
              break;
            }
            case 401: {
              errorMessage =
                error.error?.message +
                  ' Per favore <a href="/auth/login">effettua il login</a>' ||
                'Non autorizzato. Per favore <a href="/auth/login">effettua il login</a>';
              break;
            }
            case 403: {
              errorMessage = 'Accesso negato. Non hai permessi necessari';
              break;
            }
            case 404: {
              errorMessage = error.error?.message || 'Risorsa non trovata';
              break;
            }
            case 500: {
              errorMessage = 'Si è verificato un errore interno del server';
              break;
            }
          }
        }

        console.error(`Errore HTTP ${statusCode}: ${errorMessage}`);

        return throwError(() => new Error(errorMessage));
      })
    );
  }

  private formatValidationErrors(errors: any): string {
    let formattedErrors: string[] = [];

    for (const key in errors) {
      if (errors.hasOwnProperty(key)) {
        formattedErrors = formattedErrors.concat(errors[key]);
      }
    }

    // Unisci gli errori in una stringa separata da virgola
    return formattedErrors.join(', ');
  }
}
