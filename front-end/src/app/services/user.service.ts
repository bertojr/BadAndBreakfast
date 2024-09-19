import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { iUser } from '../models/i-user';
import { CrudService } from './crud.service';
import { Observable } from 'rxjs';
import { iRole } from '../models/i-role';

@Injectable({
  providedIn: 'root',
})
export class UserService extends CrudService<iUser> {
  private userUrl: string = 'User';

  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint(this.userUrl);
  }

  addRoleToUser(userId: number, roleId: number): Observable<iRole> {
    const apiUrl = this.getUrl();
    return this.http.post<iRole>(`${apiUrl}/${userId}/role/${roleId}`, null);
  }

  removeRoleFromUser(userId: number, roleId: number): Observable<void> {
    const apiUrl = this.getUrl();
    return this.http.delete<void>(`${apiUrl}/${userId}/role/${roleId}`);
  }
}
