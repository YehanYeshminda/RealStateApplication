import { Component, Input, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { NgbCollapse } from '@ng-bootstrap/ng-bootstrap';

// service
import { AuthenticationService } from 'src/app/core/service/auth.service';
import { EventService } from 'src/app/core/service/event.service';

// utility
import { changeBodyAttribute, findAllParent, findMenuItem } from '../helper/utils';

// types
import { User } from 'src/app/core/models/auth.models';
import { MenuItem } from '../models/menu.model';

// data
import { UserPermissionHttpService } from 'src/app/pages/dashboards/user-permissions/services/user-permission-http.service';
import { retry, timeout } from 'rxjs';
import { MENU_ITEMS } from '../config/menu-meta';

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.component.html',
  styleUrls: ['./left-sidebar.component.scss']
})
export class LeftSidebarComponent implements OnInit {
  userMenuItems: MenuItem[] = [
    {
      key: 'LeadsModule',
      label: 'Lead Module',
      isTitle: false,
      id: 1,
      icon: 'fe-user-check',
      collapsed: true,
      children: [
        {
          key: 'Lead',
          label: 'Leads',
          isTitle: false,
          id: 1,
          icon: 'me-1 fe-check-square',
          url: '/dashboard/lead'
        },
        {
          key: 'LeadForward',
          label: 'Lead Forward',
          isTitle: false,
          id: 2,
          icon: 'me-1 fe-phone-forwarded',
          url: '/dashboard/leadForward'
        },
        {
          key: 'LeadSegregation',
          label: 'Lead Segregation',
          isTitle: false,
          id: 3,
          icon: 'me-1 fe-external-link',
          url: '/dashboard/leadSegregation'
        }
      ],
    },
    {
      key: 'CallCenterModule',
      label: 'Call Center Module',
      isTitle: false,
      icon: 'fe-phone-call',
      collapsed: true,
      id: 4,
      children: [
        {
          key: 'Make_Call',
          label: 'Make Call',
          isTitle: false,
          icon: 'me-1 fe-phone-forwarded',
          url: '/dashboard/make-call',
          id: 1
        },
        {
          key: 'Call_List',
          label: 'Call List',
          isTitle: false,
          icon: 'me-1 mdi mdi-contacts-outline',
          url: '/dashboard/call-list',
          id: 2
        },
        {
          key: 'Call_Segregation',
          label: 'Call Segregation',
          isTitle: false,
          icon: 'me-1 mdi mdi-call-split',
          url: '/dashboard/call-segregation',
          id: 3
        },
      ]
    },

    {
      key: 'StaffModule',
      label: 'Staff Module',
      isTitle: false,
      icon: 'fe-phone-call',
      collapsed: true,
      id: 5,
      children: [
        {
          key: 'Staffs',
          label: 'Staff',
          isTitle: false,
          icon: 'me-1 fe-users',
          url: '/dashboard/staff',
          id: 1
        },
        // {
        //   key: 'Staffs',
        //   label: 'Assigned Calls (Today)',
        //   isTitle: false,
        //   icon: 'me-1 fe-users',
        //   url: '/dashboard/remove-assigned-calls',
        //   id: 2
        // }
      ]
    },
    {
      key: 'apps-email',
      label: 'Email',
      isTitle: false,
      icon: 'mdi mdi-email-outline',
      collapsed: true,
      id: 6,
      children: [
          {
            key: 'Staffs',
            label: 'Email',
            isTitle: false,
            icon: 'me-1 mdi mdi-email-outline',
            url: '/apps/email/inbox',
            id: 1
          },
      ],
    },
    {
      key: 'UserPermission',
      label: 'User Permission',
      isTitle: false,
      collapsed: true,
      id: 1,
      icon: 'fe-shield',
      children: [
        {
          key: 'UserPermission',
          label: 'User Permission',
          isTitle: false,
          icon: 'me-1 fe-shield',
          url: '/dashboard/user-permission',
          id: 1
        },
      ],
    },

    {
      key: 'ChangeDailyAmount',
      label: 'Daily Target',
      isTitle: false,
      collapsed: true,
      id: 10,
      icon: 'fe-bar-chart-2',
      children: [
        {
          key: 'ChangeDailyAmount',
          label: 'Convertions',
          isTitle: false,
          icon: 'me-1 fe-bar-chart-2',
          url: '/dashboard/perday',
          id: 1
        },
      ],
    },

    {
      key: 'EmployeePerformance',
      label: 'Employee Performance',
      isTitle: false,
      collapsed: true,
      id: 11,
      icon: 'me-1 mdi mdi-account-group',
      children: [
        {
          key: 'EmployeePerformance',
          label: 'Employee',
          isTitle: false,
          icon: 'me-1 mdi mdi-account-lock-outline',
          url: '/dashboard/employee-performance',
        }
      ]
    },
    {
      key: 'EmployeePerformance',
      label: 'Vender',
      isTitle: false,
      collapsed: true,
      id: 10,
      icon: 'fe-user-plus',
      children: [
        {
          key: 'EmployeePerformance',
          label: 'Vendor List',
          isTitle: false,
          icon: 'me-1 fe-user-plus',
          url: '/dashboard/venderlist',
          id: 1
        },
      ],
    },
    {
      key: 'Designation',
      label: 'Designation',
      isTitle: false,
      collapsed: true,
      id: 11,
      icon: 'me-1 mdi mdi-account-group',
      children: [
        {
          key: 'Designation',
          label: 'Designation',
          isTitle: false,
          icon: 'me-1 mdi mdi-account-lock-outline',
          url: '/dashboard/designation',
        }
      ]
    },
    {
      key: 'Designation',
      label: 'Vender To Service',
      isTitle: false,
      collapsed: true,
      id: 10,
      icon: 'fe-user-check',
      children: [
        {
          key: 'Designation',
          label: 'Vendor To Service List',
          isTitle: false,
          icon: 'me-1 fe-user-check',
          url: '/dashboard/vtslist',
          id: 1
        },
      ],
    },
    {
      key: 'LeadLog',
      label: 'Logs',
      isTitle: false,
      id: 11,
      collapsed: true,
      icon: 'fe-user-check',
      children: [
        {
          key: 'LeadLog',
          label: 'Lead Logs',
          isTitle: false,
          icon: 'me-1 fe-user-check',
          url: '/dashboard/leadLog',
          id: 1
        },
      ],
    },
    {
      key: 'ArchivedLeads',
      label: 'Archvied Leads',
      isTitle: false,
      id: 11,
      collapsed: true,
      icon: 'fe-user-check',
      children: [
        {
          key: 'ArchivedLeads',
          label: 'Leads',
          isTitle: false,
          icon: 'me-1 fe-user-check',
          url: '/dashboard/archivedLeads',
          id: 1
        },
      ],
    },
    {
      key: 'ArchivedLeads',
      label: 'Property Registration',
      isTitle: false,
      collapsed: true,
      id: 10,
      icon: 'fe-user-check',
      children: [
        {
          key: 'ArchivedLeads',
          label: 'Property Registration List',
          isTitle: false,
          icon: 'me-1 fe-user-check',
          url: '/dashboard/propertyregisterlist',
        }
      ]
    },
    {
      key: 'Notification',
      label: 'Notification',
      isTitle: false,
      collapsed: true,
      id: 17,
      icon: 'fe-bell',
      children: [
        {
          key: 'Notification',
          label: 'Notification List',
          isTitle: false,
          icon: 'me-1 fe-user-check',
          url: '/dashboard/notificationlist',
        }
      ]
    },
    
    {
      key: 'StaffPerformance',
      label: 'Staff Performance',
      isTitle: false,
      collapsed: true,
      id: 17,
      icon: ' fe-pie-chart',
      children: [
        {
          key: 'StaffPerformance',
          label: 'Staff Performance',
          isTitle: false,
          icon: 'me-1 fe-user-check',
          url: '/dashboard/staffperformance',
        }
      ]
    },
    // {
    //   key: 'StaffPerformance',
    //   label: 'Meeting Schedule',
    //   isTitle: false,
    //   id: 10,
    //   icon: 'mdi mdi-human-female-female',
    //   children: [
    //     {
    //       key: 'StaffPerformance',
    //       label: 'Meeting schedule',
    //       isTitle: false,
    //       icon: 'me-1 mdi mdi-human-female-female',
    //       url: '/dashboard/meeting',
    //       id: 1
    //     },
    //   ],
    // },
  ]

  @Input() includeUserProfile: boolean = false;

  leftSidebarClass = 'sidebar-enable';
  activeMenuItems: string[] = [];
  loggedInUser: User | null = {};
  menuItems: MenuItem[] = [];
  permissions: string[] = [];

  constructor (
    router: Router,
    private authService: AuthenticationService,
    private eventService: EventService, private userPermissionHtppService: UserPermissionHttpService) {
    router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this._activateMenu(); //actiavtes menu
        this.hideMenu(); //hides leftbar on change of route
      }
    });
  }

  ngOnInit(): void {
    // this.initMenu();
    this.loggedInUser = this.authService.currentUser();
    this.loadAllUserPermissions();
  }

  ngOnChanges(): void {
    if (this.includeUserProfile) {
      changeBodyAttribute('data-sidebar-user', 'true');
    }
    else {
      changeBodyAttribute('data-sidebar-user', 'false');
    }
  }

  /**
   * On view init - activating menuitems
   */
  ngAfterViewInit() {
    setTimeout(() => {
      this._activateMenu();
    });
  }
  /**
   * initialize menuitems
   */
  initMenu(isEmpty: boolean): void {
    // this.menuItems = MENU_ITEMS;

    if (isEmpty) {
      this.menuItems = [];
      return;
    } else {
      this.menuItems = this.userMenuItems;
      return;
    }
  }

  loadAllUserPermissions() {
    this.userPermissionHtppService.getAllUserPermissionsForHome()
  .pipe(
    timeout(5000),
    retry(3),
  )
  .subscribe({
    next: response => {
      if (response.result.length === 0) {
        this.menuItems = [];
        this.initMenu(true);
        return;
      }
      if (response && response.result && Array.isArray(response.result)) {
        const allowedKeys = response.result.map(item => item);

        this.userMenuItems = this.userMenuItems.filter(menuItem => {
          if (menuItem.children) {
            menuItem.children = menuItem.children.filter((childItem: any) => {
              if (!childItem.key) {
                return true;
              }
              return allowedKeys.includes(childItem.key);
            });
            return menuItem.children.length > 0;
          }
          return true;
        });

        this.userMenuItems.unshift(
          {
            key: 'primarymodules',
            label: 'MAIN MODULES',
            isTitle: true,
            id: 1
          },
          {
            key: 'dashboard',
            label: 'Dashboard',
            isTitle: false,
            icon: 'mdi mdi-view-dashboard-outline',
            url: '/dashboard',
            id: 2
          }
        );

        this.initMenu(false);
      } else {
        console.error('Invalid or missing response format.');
      }
    },
    error: err => {
      console.error('Error fetching user permissions:', err);
    }
  });
  }

  /**
   * Returns true or false if given menu item has child or not
   * @param item menuItem
   */
  hasSubmenu(menu: MenuItem): boolean {
    return menu.children ? true : false;
  }

  /**
   * activates menu
   */
  _activateMenu(): void {
    const div = document.getElementById('side-menu');
    let matchingMenuItem = null;

    if (div) {
      let items: any = div.getElementsByClassName('side-nav-link-ref');
      for (let i = 0; i < items.length; ++i) {
        if (window.location.pathname === items[i].pathname) {
          matchingMenuItem = items[i];
          break;
        }
      }

      if (matchingMenuItem) {
        const mid = matchingMenuItem.getAttribute('data-menu-key');
        const activeMt = findMenuItem(this.menuItems, mid);
        if (activeMt) {

          const matchingObjs = [activeMt['key'], ...findAllParent(this.menuItems, activeMt)];

          this.activeMenuItems = matchingObjs;

          this.menuItems.forEach((menu: MenuItem) => {
            menu.collapsed = !matchingObjs.includes(menu.key!);
          });
        }
      }
    }
  }

  /**
   * toggles open menu
   * @param menuItem clicked menuitem
   * @param collapse collpase instance
   */
  toggleMenuItem(menuItem: MenuItem, collapse: NgbCollapse): void {
    collapse.toggle();
    let openMenuItems: string[];
    if (!menuItem.collapsed) {
      openMenuItems = ([menuItem['key'], ...findAllParent(this.menuItems, menuItem)]);
      this.menuItems.forEach((menu: MenuItem) => {
        if (!openMenuItems.includes(menu.key!)) {
          menu.collapsed = true;
        }
      })
    }

  }

  /**
   * Hides the menubar
   */
  hideMenu() {
    document.body.classList.remove('sidebar-enable');
  }
}
