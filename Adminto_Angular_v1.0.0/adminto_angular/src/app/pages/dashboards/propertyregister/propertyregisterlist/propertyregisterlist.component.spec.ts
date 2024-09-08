import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyregisterlistComponent } from './propertyregisterlist.component';

describe('PropertyregisterlistComponent', () => {
  let component: PropertyregisterlistComponent;
  let fixture: ComponentFixture<PropertyregisterlistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PropertyregisterlistComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PropertyregisterlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
