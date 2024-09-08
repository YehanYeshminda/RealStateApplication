import { Component, Input, OnInit } from '@angular/core';
// types
import { ApexChartOptions } from 'src/app/pages/charts/apex/apex-chart.model';

@Component({
  selector: 'app-statistics-chart',
  templateUrl: './statistics-chart.component.html',
  styleUrls: ['./statistics-chart.component.scss']
})
export class StatisticsChartComponent implements OnInit {
  @Input() chartData: number[] = [];
  @Input() chartName: string = "default name"
  chartOptions: Partial<ApexChartOptions> = {};

  constructor () { }

  ngOnInit(): void {
    this.chartOptions = {
      series: [
        {
          name: '',
          data: this.chartData,
        },
      ],
      chart: {
        type: 'bar',
        height: 268,
        toolbar: {
          show: false,
        },
      },
      plotOptions: {
        bar: {
          columnWidth: '20%',
        },
      },
      dataLabels: {
        enabled: false,
      },
      stroke: {
        show: false,
      },
      xaxis: {
        categories: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
        labels: {
          style: {
            colors: '#adb5bd',
          },
        },
      },
      yaxis: {
        labels: {
          style: {
            colors: '#adb5bd',
          },
        },
      },
      grid: {
        show: false,
        padding: {
          top: 0,
          right: 0,
          bottom: 0,
          left: 0,
        },
      },
      fill: {
        opacity: 1,
      },
      colors: ['#188ae2'],
      tooltip: {
        theme: 'dark',
      },
    }
  }

}
