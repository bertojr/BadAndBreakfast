import { AdditionalServiceService } from './../../../services/additional-service.service';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { iAdditionalService } from '../../../models/i-additional-service';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-additional-service-edit',
  templateUrl: './additional-service-edit.component.html',
  styleUrl: './additional-service-edit.component.scss',
})
export class AdditionalServiceEditComponent {
  @Input() service!: iAdditionalService;
  @Output() serviceUpdated = new EventEmitter<void>();
  errorMessage: string | null = null;
  isEditMode: boolean = false;

  constructor(
    private additionalServiceSvc: AdditionalServiceService,
    private activeModale: NgbActiveModal
  ) {}

  onSubmit(form: NgForm) {
    if (form.valid) {
      if (this.service.serviceID) {
        // modifica esistente
        this.additionalServiceSvc
          .update(this.service.serviceID, this.service)
          .subscribe({
            next: () => {
              this.errorMessage = null;
              this.serviceUpdated.emit();
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
        const newService: Partial<iAdditionalService> = {
          name: this.service.name,
          description: this.service.description,
          unitPrice: this.service.unitPrice,
        };
        this.additionalServiceSvc.create(newService).subscribe({
          next: () => {
            this.errorMessage = null;
            this.serviceUpdated.emit();
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
  closeModal(): void {
    this.activeModale.close();
  }
}
