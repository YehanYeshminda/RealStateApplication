import Swal from 'sweetalert2';

export function errorNotification(error: string) {
	Swal.fire({
		icon: 'error',
		title: 'Oops...',
		text: error,
	});
}

export function errorNotificationList(error: string) {
	Swal.fire({
		icon: 'error',
		title: 'Oops...',
		text: error,
	});
}

export function successNotification(message: string) {
	Swal.fire({
		icon: 'success',
		title: 'Success',
		text: message,
	});
}

export function warningNotification(message: string) {
	Swal.fire({
		icon: 'warning',
		title: 'Warning',
		text: message,
	});
}

export function infoNotification(message: string) {
	Swal.fire({
		icon: 'info',
		title: 'Info',
		text: message,
	});
}

export function confirmNotification(message: string) {
	return Swal.fire({
		icon: 'info',
		title: 'Are you sure?',
		text: message,
		showCancelButton: true,
		confirmButtonText: 'Yes!',
		cancelButtonText: "No, Don't Save it!",
	});
}

export function confirmDeleteNotification(message: string) {
	return Swal.fire({
		icon: 'warning',
		title: 'Are you sure?',
		text: message,
		showCancelButton: true,
		confirmButtonText: 'Yes, Delete it!',
		cancelButtonText: "No, Don't Delete it!",
	});
}

export function confirmUpdateNotification(message: string) {
	return Swal.fire({
		icon: 'warning',
		title: 'Are you sure?',
		text: message,
		showCancelButton: true,
		confirmButtonText: 'Yes, Update it!',
		cancelButtonText: "No, Don't Update it!",
	});
}

export function confirmApproveNotification(message: string) {
	return Swal.fire({
		icon: 'warning',
		title: 'Are you sure?',
		text: message,
		showCancelButton: true,
		confirmButtonText: 'Yes, Approve it!',
		cancelButtonText: "No, Don't Approve it!",
	});
}

export function confirmRejectNotification(message: string) {
	return Swal.fire({
		icon: 'warning',
		title: 'Are you sure?',
		text: message,
		showCancelButton: true,
		confirmButtonText: 'Yes, Reject it!',
		cancelButtonText: "No, Don't Reject it!",
	});
}

export function confirmCancelNotification(message: string) {
	return Swal.fire({
		icon: 'warning',
		title: 'Are you sure?',
		text: message,
		showCancelButton: true,
		confirmButtonText: 'Yes, Cancel it!',
		cancelButtonText: "No, Don't Cancel it!",
	});
}
