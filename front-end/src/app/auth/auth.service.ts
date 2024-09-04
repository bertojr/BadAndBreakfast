import { iAuthData } from './../models/i-auth-data';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { iUser } from '../models/i-user';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';
import { iAuthResponse } from '../models/i-auth-response';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // ci permette di lavora meglio con i JWT
  jwtHelper: JwtHelperService = new JwtHelperService();

  // contiene l'oggetto user con tutte le info, se null vuol dire che non è loggato
  authSubject = new BehaviorSubject<null | iUser>(null);

  // uso questa variabile nelle guard e nell'interceptor per sapere se l'utente è loggoato o meno
  syncIsLoggedIn: boolean = false;

  // contiene i dati dell'utente loggato oppure null
  user$ = this.authSubject.asObservable();

  // restituiesce true se l'utente è loggato, false non è loggato
  isLoggedIn$ = this.user$.pipe(
    map((user) => !!user),
    tap((user) => (this.syncIsLoggedIn = user))
  );

  constructor(private http: HttpClient) {}

  loginUrl: string = `${environment.apiUrl}/auth/login`;
  registerUrl: string = `${environment.apiUrl}/auth/register`;

  // metodo per effettuare la registrazione
  register(newUser: Partial<iUser>): Observable<iAuthResponse> {
    return this.http.post<iAuthResponse>(this.registerUrl, newUser);
  }

  // metodo che effettua la chiamata per effettuare il login
  login(authData: iAuthData): Observable<iAuthResponse> {
    return this.http.post<iAuthResponse>(this.loginUrl, authData).pipe(
      tap((data) => {
        // quando l'utente è loggato comunico i suoi dati al subject
        this.authSubject.next(data.user);

        // salvo anche i suoi dati in localstorage
        localStorage.setItem('accessData', JSON.stringify(data));
      })
    );
  }
}
