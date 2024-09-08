import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyassignlistComponent } from './propertyassignlist.component';

describe('PropertyassignlistComponent', () => {
  let component: PropertyassignlistComponent;
  let fixture: ComponentFixture<PropertyassignlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PropertyassignlistComponent]
    });
    fixture = TestBed.createComponent(PropertyassignlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
