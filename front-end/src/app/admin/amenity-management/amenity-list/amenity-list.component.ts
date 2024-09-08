import { Component } from '@angular/core';
import { iAmenity } from '../../../models/i-amenity';
import { AmenityService } from '../../../services/amenity.service';
import { AmenityEditComponent } from '../amenity-edit/amenity-edit.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-amenity-list',
  templateUrl: './amenity-list.component.html',
  styleUrl: './amenity-list.component.scss',
})
export class AmenityListComponent {
  amenities: iAmenity[] = [];
  errorMessage: string | null = '';

  constructor(private amenitySvc: AmenityService, private modalSvc: NgbModal) {}

  ngOnInit(): void {
    this.loadAmenity();
  }

  private loadAmenity(): void {
    this.amenitySvc.getAll().subscribe({
      next: (amenities) => {
        this.amenities = amenities;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la ricerca';
      },
    });
  }

  deleteUser(id: number): void {
    this.amenitySvc.delete(id).subscribe({
      next: () => {
        this.amenities = this.amenities.filter(
          (amenity) => amenity.amenityID !== id
        );
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la eliminazione';
      },
    });
  }

  openModal(amenity?: iAmenity) {
    const modalRef = this.modalSvc.open(AmenityEditComponent);
    modalRef.componentInstance.amenity = amenity ? amenity : {};

    modalRef.componentInstance.amenityUpdated.subscribe(() => {
      this.loadAmenity();
    });
  }
}
