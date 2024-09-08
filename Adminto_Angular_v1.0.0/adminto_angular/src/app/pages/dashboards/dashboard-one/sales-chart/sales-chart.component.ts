import { Component, Input, OnInit } from '@angular/core';
// types
import { ApexChartOptions } from 'src/app/pages/charts/apex/apex-chart.model';

@Component({
  selector: 'app-sales-chart',
  templateUrl: './sales-chart.component.html',
  styleUrls: ['./sales-chart.component.scss']
})
export class SalesChartComponent implements OnInit {
  @Input() chartData: number[] = [];
  chartOptions: Partial<ApexChartOptions> = {};
  @Input() chartTitle: string = 'Default title';

  constructor () { }

  ngOnInit(): void {
    this.chartOptions = {
      series: this.chartData,
      chart: {
        type: 'donut',
        height: 315,
      },
      plotOptions: {
        pie: {
          expandOnClick: true,
          donut: {
            labels: {
              show: true,
              name: {
                show: true,
                formatter: (val: string) => {
                  return val;
                },
                offsetY: 4,
                color: '#98a6ad',
              },
              value: {
                show: true,
                formatter: (val: string) => {
                  return val;
                },
                color: '#98a6ad',
              }
            },
          },
        },
      },
      dataLabels: {
        enabled: false,
      },
      colors: ['#6658dd', '#ff8acc', '#35b8e0'],
      legend: {
        show: true,
        position: 'bottom',
        height: 40,
        labels: {
          useSeriesColors: true,
        },
      },
      labels: ['Calls To Make', 'Calls Made Today', 'Conversions Today'],
      tooltip: {
        enabled: false,
      },
    }
  }

}
