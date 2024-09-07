import { Injectable } from '@angular/core';
import { CrudService } from './crud.service';
import { iRoom } from '../models/i-room';
import { HttpClient } from '@angular/common/http';
import { iAmenity } from '../models/i-amenity';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RoomService extends CrudService<iRoom> {
  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint('Room');
  }

  addAmenityToRoom(roomId: number, amenityId: number): Observable<iAmenity> {
    const apiUrl = this.getUrl();
    return this.http.post<iAmenity>(
      `${apiUrl}/${roomId}/amenity/${amenityId}`,
      null
    );
  }

  removeAmenityFromRoom(roomId: number, amenityId: number): Observable<void> {
    const apiUrl = this.getUrl();
    return this.http.delete<void>(`${apiUrl}/${roomId}/amenity/${amenityId}`);
  }
}
