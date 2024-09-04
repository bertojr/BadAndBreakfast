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

  constructor(private http: HttpClient) {
    this.restoreUser();
  }

  loginUrl: string = `${environment.apiUrl}/Auth/login`;
  registerUrl: string = `${environment.apiUrl}/Auth/register`;

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

  // metdo per effettuare il logout
  logout(): void {
    this.authSubject.next(null);
    localStorage.removeItem('accessData');
  }

  // metodo per effettuare l'auto logout alla scadenza del token
  autoLogout(): void {
    const accessData = this.getAccessData();

    if (!accessData) return;

    const expDate = this.jwtHelper.getTokenExpirationDate(
      accessData.token
    ) as Date;

    const expMs = expDate.getTime() - new Date().getTime();

    setTimeout(() => this.logout(), expMs);
  }

  // metodo per recuperare i dati di accesso
  getAccessData(): iAuthResponse | null {
    const accessDataJson = localStorage.getItem('accessData');

    if (!accessDataJson) return null;

    const accessData: iAuthResponse = JSON.parse(accessDataJson);

    return accessData;
  }

  restoreUser(): void {
    const accessData = this.getAccessData();

    // controllo se non c'è un utente loggato
    if (!accessData) return;

    // controllo se il token è scaduto
    if (this.jwtHelper.isTokenExpired(accessData.token)) return;

    this.authSubject.next(accessData.user);
    this.autoLogout();
  }
}
