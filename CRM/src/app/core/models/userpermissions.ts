import { AuthDetails } from "src/app/shared/models/authDetails";

export interface UserPermission {
    AuthDto: AuthDetails;
    location: string;
    event: string;
}

