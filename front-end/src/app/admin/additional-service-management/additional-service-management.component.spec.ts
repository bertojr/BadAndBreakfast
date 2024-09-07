import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalServiceManagementComponent } from './additional-service-management.component';

describe('AdditionalServiceManagementComponent', () => {
  let component: AdditionalServiceManagementComponent;
  let fixture: ComponentFixture<AdditionalServiceManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdditionalServiceManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdditionalServiceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
