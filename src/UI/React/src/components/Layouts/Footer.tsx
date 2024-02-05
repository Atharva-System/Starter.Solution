const Footer = () => {
    return (
        <div>
            <p className="dark:text-white-dark text-center ltr:sm:text-left rtl:sm:text-right pt-6">
                © {new Date().getFullYear()}. Vristo All rights reserved.
            </p>
        </div>
    );
};

export default Footer;
