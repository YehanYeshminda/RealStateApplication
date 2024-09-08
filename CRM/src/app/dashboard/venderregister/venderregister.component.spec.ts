import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VenderregisterComponent } from './venderregister.component';

describe('VenderregisterComponent', () => {
  let component: VenderregisterComponent;
  let fixture: ComponentFixture<VenderregisterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VenderregisterComponent]
    });
    fixture = TestBed.createComponent(VenderregisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
