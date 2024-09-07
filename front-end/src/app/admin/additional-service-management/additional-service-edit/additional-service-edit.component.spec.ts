import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalServiceEditComponent } from './additional-service-edit.component';

describe('AdditionalServiceEditComponent', () => {
  let component: AdditionalServiceEditComponent;
  let fixture: ComponentFixture<AdditionalServiceEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdditionalServiceEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdditionalServiceEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
