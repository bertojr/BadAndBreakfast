import { Component } from '@angular/core';
import { iAmenity } from '../../../models/i-amenity';
import { AmenityService } from '../../../services/amenity.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-amenity-edit',
  templateUrl: './amenity-edit.component.html',
  styleUrl: './amenity-edit.component.scss',
})
export class AmenityEditComponent {
  amenity: iAmenity = {
    amenityID: 0,
    name: '',
  };
  errorMessage: string | null = null;
  isEditMode: boolean = false;

  constructor(
    private amenitySvc: AmenityService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // Controlla se stiamo modificando una comodità esistente
    this.route.params.subscribe((params: any) => {
      const amenityId = params.id;
      if (amenityId) {
        this.isEditMode = true;
        this.loadAmenity(amenityId);
      }
    });
  }

  private loadAmenity(amenityId: number): void {
    this.amenitySvc.getById(amenityId).subscribe({
      next: (amenity) => {
        this.errorMessage = null;
        this.amenity = amenity;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Errore durante il caricamento della camera';
      },
    });
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      if (this.isEditMode) {
        // modifica esistente
        this.amenitySvc.update(this.amenity.amenityID, this.amenity).subscribe({
          next: () => {
            this.errorMessage = null;
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
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante il login';
          },
        });
      }
    }
  }
}
