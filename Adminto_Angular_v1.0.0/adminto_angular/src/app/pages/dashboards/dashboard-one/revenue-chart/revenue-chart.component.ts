import { Component, Input, OnInit } from '@angular/core';

// types
import { ApexChartOptions } from 'src/app/pages/charts/apex/apex-chart.model';

@Component({
  selector: 'app-revenue-chart',
  templateUrl: './revenue-chart.component.html',
  styleUrls: ['./revenue-chart.component.scss']
})
export class RevenueChartComponent implements OnInit {
  chartOptions: Partial<ApexChartOptions> = {};

  @Input() assignedCalls: number[] = [];
  @Input() conversions: number[] = [];

  constructor () { }

  getYAxixFigures(): number {
    const maxAssignedCalls = this.assignedCalls.length > 0 ? Math.max(...this.assignedCalls) : 0;
    const maxConversions = this.conversions.length > 0 ? Math.max(...this.conversions) : 0;
    return Math.max(maxAssignedCalls, maxConversions);
  }

  ngOnInit(): void {
    this.chartOptions = {
      series: [
        {
          name: 'Assigned Calls',
          type: 'area',
          data: this.assignedCalls,
        },
        {
          name: 'Conversions',
          type: 'line',
          data: this.conversions,
        },
      ],
      chart: {
        height: 268,
        type: 'line',
        toolbar: {
          show: false,
        },
        stacked: false,
        zoom: {
          enabled: false,
        },
      },
      stroke: {
        curve: 'smooth',
        width: [3, 3],
      },
      dataLabels: {
        enabled: false,
      },
      legend: {
        show: false,
      },
      fill: {
        type: 'solid',
        opacity: [0, 1],
      },
      colors: ['#3cc469', '#188ae2'],
      xaxis: {
        categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
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
        tickAmount: 4,
        min: 0,
        max: this.getYAxixFigures(),
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
          bottom: 0,
        },
      },
      tooltip: {
        theme: 'dark',
      },
    }
  }

}
