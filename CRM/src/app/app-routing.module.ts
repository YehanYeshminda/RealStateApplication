
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NotfoundComponent } from './core/notfound/notfound.component';

const routes: Routes = [
	{
		path: '', component: LoginComponent
	},
	{
		path: 'dashboard', loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
	},
	{
		path: "**",
		component: NotfoundComponent
	}
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }

// const routes: Routes = [
// 	{
// 		path: '',
// 		component: LoginComponent,
// 	},
// 	{
// 		path: 'dashboard',
// 		loadChildren: () =>
// 			import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
// 	},
// 	{
// 		path: "**",
// 		redirectTo: '',
// 		component: LoginComponent
// 	}
// ];

// @NgModule({
// 	imports: [RouterModule.forRoot(routes),CommonModule],
// 	exports: [RouterModule],
// })
// export class AppRoutingModule { }



