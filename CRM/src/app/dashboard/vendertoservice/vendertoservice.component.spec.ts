import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendertoserviceComponent } from './vendertoservice.component';

describe('VendertoserviceComponent', () => {
  let component: VendertoserviceComponent;
  let fixture: ComponentFixture<VendertoserviceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VendertoserviceComponent]
    });
    fixture = TestBed.createComponent(VendertoserviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
