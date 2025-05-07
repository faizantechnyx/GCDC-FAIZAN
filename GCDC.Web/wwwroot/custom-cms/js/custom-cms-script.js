angular.module("umbraco").run(function ($rootScope) {
    // Wait for the backoffice view to be fully loaded
    $rootScope.$on('$viewContentLoaded', function () {

        // Remove the "Help" menu item (update the selector based on the attribute for Help)
        const helpElement = document.querySelector('li[data-element="global-help"]');
        if (helpElement) {
            helpElement.innerHTML = "";
        }


        // Find the logo element and remove it
        const logoElement = document.querySelector(".umb-app-header__logo");
        if (logoElement) {
            logoElement.innerHTML = "";
        }

        // Create a MutationObserver to monitor changes
        const observer = new MutationObserver(() => {
            // Check if the "Forms" section has the "current" class
            const formsSection = document.querySelector('li[data-element="section-forms"].current');
            if (formsSection) {
                // Check if the target div is present
                const formElement = document.querySelector('div[ng-show="vm.currentVersion"]');
                if (formElement) {
                    formElement.remove(); // Remove the element
                    observer.disconnect(); // Stop observing once the task is done
                }
            }

            // Check if the "Packages" section has the "current" class
            const packagesSection = document.querySelector('li[data-element="section-packages"].current');
            if (packagesSection) {
                // Check if the target div is present
                const packElement = document.querySelector('div[class="umb-package-list__item-description"]');
                if (packElement) {
                    packElement.remove(); // Remove the element
                    observer.disconnect(); // Stop observing once the task is done
                }
            }

            // Select the notifications wrapper
            const notificationsWrapper = document.querySelector('div.umb-notifications ul[ng-if="notifications"]');
            if (notificationsWrapper) {
                // Use MutationObserver to dynamically handle notifications
                const observer = new MutationObserver((mutationsList) => {
                    mutationsList.forEach((mutation) => {
                        // Check if nodes were added (new notifications)
                        mutation.addedNodes.forEach((node) => {
                            if (node.nodeType === Node.ELEMENT_NODE && node.matches('li.umb-notifications__notification')) {
                                // Locate the notification URL element
                                const notificationMessage = node.querySelector('div[ng-if="!notification.url"]');
                                const urlElement = notificationMessage?.querySelector('i');
                                if (urlElement) {
                                    // Remove notification
                                    const nextSibling = urlElement.nextSibling;
                                    if (nextSibling && nextSibling.nodeName == "BR") {
                                        nextSibling.remove();
                                    }
                                    urlElement.remove();
                                }
                            }
                        });
                    });
                });

                // Observe changes to the notifications list
                observer.observe(notificationsWrapper, { childList: true, subtree: true });
            }

        });

        // Start observing changes in the document body (or a more specific container if applicable)
        observer.observe(document.body, {
            childList: true, // Watch for additions/removals of child nodes
            subtree: true    // Watch within all descendants
        });
    });
});
