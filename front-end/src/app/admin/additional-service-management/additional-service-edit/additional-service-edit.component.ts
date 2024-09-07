import { AdditionalServiceService } from './../../../services/additional-service.service';
import { Component } from '@angular/core';
import { iAdditionalService } from '../../../models/i-additional-service';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-additional-service-edit',
  templateUrl: './additional-service-edit.component.html',
  styleUrl: './additional-service-edit.component.scss',
})
export class AdditionalServiceEditComponent {
  service: iAdditionalService = {
    serviceID: 0,
    name: '',
    unitPrice: 0,
  };
  errorMessage: string | null = null;
  isEditMode: boolean = false;

  constructor(
    private additionalServiceSvc: AdditionalServiceService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      const serviceId = params.id;
      if (serviceId) {
        this.isEditMode = true;
        this.loadService(serviceId);
      }
    });
  }

  private loadService(serviceId: number): void {
    this.additionalServiceSvc.getById(serviceId).subscribe({
      next: (service) => {
        this.errorMessage = null;
        this.service = service;
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
        this.additionalServiceSvc
          .update(this.service.serviceID, this.service)
          .subscribe({
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
        const newService: Partial<iAdditionalService> = {
          name: this.service.name,
          description: this.service.description,
          unitPrice: this.service.unitPrice,
        };
        this.additionalServiceSvc.create(newService).subscribe({
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
