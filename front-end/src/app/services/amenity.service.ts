import { Injectable } from '@angular/core';
import { CrudService } from './crud.service';
import { iAmenity } from '../models/i-amenity';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AmenityService extends CrudService<iAmenity> {
  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint('Amenity');
  }
}
