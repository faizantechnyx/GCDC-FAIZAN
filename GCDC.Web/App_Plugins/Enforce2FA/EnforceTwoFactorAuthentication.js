(() => {
    angular.module("umbraco").run(["authResource", "twoFactorLoginResource", "editorService", "notificationsService", "eventsService", async (authResource, twoFactorLoginResource, editorService, notificationsService, eventsService) => {
        const state = {};

        const init = async (firstInit) => {
            "app.ready"
            state.currentUser = await authResource.getCurrentUser();
            state.twoFactorAuthProviders = await twoFactorLoginResource.get2FAProvidersForUser(state.currentUser.id);

            if (firstInit && !twoFactorIsEnabled()) {
                configureTwoFactor();
            }
        };

        const twoFactorIsEnabled = () => {
            return !!state.twoFactorAuthProviders.find(provider => provider.isEnabledOnUser);
        };

        const configureTwoFactor = () => {

            editorService.open({
                create: true,
                user: state.currentUser,
                isCurrentUser: true,
                size: "small",
                view: "views/common/infiniteeditors/twofactor/configuretwofactor.html",
                close: async () => {
                    await init();

                    if (twoFactorIsEnabled()) {
                        notificationsService.success("Two-Factor Auth", "You have successfully configured two-factor authentication.");
                        editorService.close();
                    } else {
                        notificationsService.error("Two-Factor Auth", "You must configure two-factor authentication before continuing.");
                    }
                }
            });
        };

        eventsService.on("app.ready", (name, args) => {
            init(true);
        });
    }]);

    var usergrp_allowed_disable2FA = ["Admin"];
    angular.module("umbraco").run(function ($rootScope, authResource, notificationsService) {
        // Wait for the backoffice view to be fully loaded
        $rootScope.$on('$viewContentLoaded', async function () {
            try {
                //console.log("new event spoofing called");
                // Fetch the current user
                const currentUser = await authResource.getCurrentUser();
                // Create a MutationObserver to monitor changes
                const observer = new MutationObserver(() => {
                    //console.log("MutationObserver called");
                    // Check if the user belongs to restricted groups
                    var removebtn = true;

                    for (var i = 0; i < usergrp_allowed_disable2FA.length; i++) {
                        for (var j = 0; j < currentUser.userGroups.length; j++) {
                            if (usergrp_allowed_disable2FA[i].toLowerCase() === currentUser.userGroups[j].toLowerCase()) {
                                removebtn = false;
                                break;
                            }
                        }
                        if (!removebtn) {
                            break;
                        }
                    }
                    if (removebtn) {
                        const _2fabtn = document.querySelector('umb-button[label-key="user_configureTwoFactor"]');
                        if (_2fabtn) {
                            _2fabtn.style.display = "none";
                        }
                    }
                });
                // Start observing changes in the document body (or a more specific container if applicable)
                observer.observe(document.body, {
                    childList: true, // Watch for additions/removals of child nodes
                    subtree: true    // Watch within all descendants
                });
            } catch (error) {
                // Log an error if the current user fetch fails
                notificationsService.error("Error", "Failed to fetch current user: " + error.message);
            }
        });
    });
})();