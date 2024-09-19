import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalServiceListComponent } from './additional-service-list.component';

describe('AdditionalServiceListComponent', () => {
  let component: AdditionalServiceListComponent;
  let fixture: ComponentFixture<AdditionalServiceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdditionalServiceListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdditionalServiceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
