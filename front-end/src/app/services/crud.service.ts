import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environments';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CrudService<T> {
  private baseApiUrl: string = environment.apiUrl;
  private apiUrl: string = '';

  constructor(protected http: HttpClient) {}

  setEndpoint(endpoint: string): void {
    this.apiUrl = `${this.baseApiUrl}/${endpoint}`;
  }
  getUrl(): string {
    return this.apiUrl;
  }

  // recupera tutta l'entità
  getAll(): Observable<T[]> {
    return this.http.get<T[]>(this.apiUrl);
  }

  // recupera l'entità con l'id nel parmaetro
  getById(id: number): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${id}`);
  }

  // crea una nuova entità
  create(item: Partial<T>): Observable<T> {
    return this.http.post<T>(this.apiUrl, item);
  }

  // aggiorna una entità esistente
  update(id: number, item: T): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${id}`, item);
  }

  // elimina un'entità
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
