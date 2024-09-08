import { Component, EventEmitter, Input, Output } from '@angular/core';
import { iAmenity } from '../../../models/i-amenity';
import { AmenityService } from '../../../services/amenity.service';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-amenity-edit',
  templateUrl: './amenity-edit.component.html',
  styleUrl: './amenity-edit.component.scss',
})
export class AmenityEditComponent {
  @Input() amenity!: iAmenity;
  @Output() amenityUpdated = new EventEmitter<void>();
  errorMessage: string | null = null;

  constructor(
    private amenitySvc: AmenityService,
    private activeModal: NgbActiveModal
  ) {}

  onSubmit(form: NgForm) {
    if (form.valid) {
      if (this.amenity.amenityID) {
        // modifica esistente
        this.amenitySvc.update(this.amenity.amenityID, this.amenity).subscribe({
          next: () => {
            this.errorMessage = null;
            this.amenityUpdated.emit();
            this.closeModal();
          },
          error: (error) => {
            this.errorMessage =
              error.message ||
              "Si è verificato un errore durante l'aggiornamento della camera";
          },
        });
      } else {
        // creazione
        const newAmenity: Partial<iAmenity> = {
          name: this.amenity.name,
          description: this.amenity.description,
        };
        this.amenitySvc.create(newAmenity).subscribe({
          next: () => {
            this.errorMessage = null;
            this.amenityUpdated.emit();
            this.closeModal();
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante il login';
          },
        });
      }
    }
  }

  closeModal() {
    this.activeModal.close();
  }
}
