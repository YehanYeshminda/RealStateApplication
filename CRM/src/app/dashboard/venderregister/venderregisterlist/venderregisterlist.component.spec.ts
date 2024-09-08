import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VenderregisterlistComponent } from './venderregisterlist.component';

describe('VenderregisterlistComponent', () => {
  let component: VenderregisterlistComponent;
  let fixture: ComponentFixture<VenderregisterlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VenderregisterlistComponent]
    });
    fixture = TestBed.createComponent(VenderregisterlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
